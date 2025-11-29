import { WebSocketServer } from 'ws';
import http from 'http';
import fs from 'fs';
import path from 'path';

const wss = new WebSocketServer({ port: 12001 });

// The rooms
const rooms = new Map();
const roomNames = new Map();
const roomCount = new Map();

// Broadcast to everyone else in room
function broadcastToRoom(roomId, sender, message) {
  const clients = rooms.get(roomId);
  if (!clients) return;

  for (const client of clients) {
    if (client !== sender && client.readyState === client.OPEN) {
      client.send(message);
    }
  }
}
function sendLobbyInfo(roomId) {
  broadcastToRoom(roomId, null, JSON.stringify({title: "lobby", id: "-1", content: {names: Array.from(roomNames.get(roomId)) }}))
}

wss.on('connection', (ws) => {
  ws.userData = { name: null, roomId: null };

  ws.on('message', (raw) => {
    let msg;

    try {
      msg = JSON.parse(raw);
    } catch (err) {
      console.error("Invalid JSON:", raw);
      return;
    }

    const { title, content, id } = msg;

    // Handle init
    if (title === "init") {
      const { name, roomId } = content;

      ws.userData = { name, roomId };

      if (!rooms.has(roomId)) {
        rooms.set(roomId, new Set());

        //telemetry
        roomNames.set(roomId, new Set());
        roomCount.set(roomId, 0);
      }
      rooms.get(roomId).add(ws);
      
      //telemetry
      roomNames.get(roomId).add(name);

      console.log(`${name} joined room ${roomId}`);

      sendLobbyInfo(roomId)
      
      return;
    }

    const { roomId } = ws.userData;
    if (!roomId) return;
    
    
    broadcastToRoom(roomId, ws, raw.toString());

    roomCount.set(roomId, roomCount.get(roomId) + 1);
  });

  ws.on('close', () => {
    const { roomId } = ws.userData;

    if (roomId && rooms.has(roomId)) {
      rooms.get(roomId).delete(ws);
      roomNames.get(roomId).delete(ws.userData.name);

      if (rooms.get(roomId).size === 0) {
        rooms.delete(roomId);
        roomNames.delete(roomId);
        roomCount.delete(roomId);
      }
      else {
        sendLobbyInfo(roomId)
      }
    }
  });
});


const server = http.createServer((req, res) => {
  // Build telemetry report
  let report = "";

  const sortedRooms = [...roomCount.entries()]
    .sort((a, b) => b[1] - a[1]); 

  for (const [roomId, count] of sortedRooms) {
    const names = [...(roomNames.get(roomId) || [])].join(", ");

    report += `================\n`;
    report += `Room Name: ${roomId}\n`;
    report += `Name List: ${names}\n`;
    report += `Message Count: ${count}\n\n`;
  }

  if (report === "") {
    report = "No rooms active yet.";
  }

  // Send as plain text file
  res.writeHead(200, {
    "Content-Type": "text/plain"
  });

  res.end(report);
});

console.log("WebSocket server running on ws://localhost:12001");
server.listen(12002, () => console.log("Site running at http://localhost:12002"));
