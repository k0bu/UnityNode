//passing argument within the second parenthesis.
const io = require('socket.io')(process.env.PORT || 52300);

//Custom Classes
const Player = require('./Classes/Player.js');


console.log('Server has started');

let players = [];
let sockets = [];

io.on('connection', function(socket){
    console.log('Connection Made!');

    let player = new Player();
    let thisPlayerID = player.id;

    players[thisPlayerID] = player;
    sockets[thisPlayerID] = socket;

    //Tell the client that this is our id for the server
    socket.emit('register', {id: thisPlayerID} );
    socket.emit('spawn', player);//Tell myself I have spawned
    socket.broadcast.emit('spawn', player);//Tell other I have spawned

    //Tell myself about everyone else in the game
    for(let playerID in players){
        if(playerID !== thisPlayerID){
            socket.emit('spawn', players[playerID]);

        }
    }

    socket.on('disconnect', function(){
        console.log('A player has disconnected');

        delete players[thisPlayerID];
        delete sockets[thisPlayerID];
        socket.broadcast.emit('disconnected', player);
    });
});
