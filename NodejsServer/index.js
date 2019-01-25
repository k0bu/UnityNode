//passing argument within the second parenthesis.
const io = require('socket.io')(process.env.PORT || 52300);

//Custom Classes
const Player = require('./Classes/Player.js');
const Bullet = require('./Classes/Bullet.js');

console.log('Server has started');

let players = [];
let sockets = [];
let bullets = [];

//Updates
setInterval( () => {
    bullets.forEach(bullet => {
        let isDestroyed = bullet.onUpdate();

        //Remove
        if(isDestroyed){
            let index = bullets.indexOf(bullet);
            if(index > -1){
                bullets.splice(index, 1);

                let returnBulletData = {
                    id: bullet.id,
                }

                for(let playerID in players){
                    sockets[playerID].emit('serverUnspawn', returnBulletData);
                }
            }
        } else {
            let returnBulletData = {
                id: bullet.id,
                position : {
                    x: bullet.position.x,
                    y: bullet.position.y,
                },
            }

            for(let playerID in players){
                sockets[playerID].emit('updatePosition', returnBulletData);
            }

        }
    });
}, 100, 0);


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

    //Positional data from client
    socket.on('updatePosition', function(data){
        player.position.x = data.position.x;
        player.position.y = data.position.y;

        socket.broadcast.emit('updatePosition', player);
    });

    socket.on('updateRotation', function(data){
        player.tankRotation = data.tankRotation;
        player.barrelRotation = data.barrelRotation;

        socket.broadcast.emit('updateRotation', player);
    });

    socket.on('fireBullet', function(data){
        let bullet = new Bullet();

        bullet.position.x = data.position.x;
        bullet.position.y = data.position.y;
        bullet.direction.x = data.direction.x;
        bullet.direction.y = data.direction.y;

        bullets.push(bullet);

        let returnBulletData = {
            name: bullet.name,
            id: bullet.id,
            position: {
                x: bullet.position.x,
                y: bullet.position.y,
            },
            direction: {
                x: bullet.direction.x,
                y: bullet.direction.y,
            },
        }

        socket.emit('serverSpawn', returnBulletData);
        socket.broadcast.emit('serverSpawn', returnBulletData);
    });





    socket.on('disconnect', function(){
        console.log(`A player (ID: ${thisPlayerID}) has disconnected`);

        delete players[thisPlayerID];
        delete sockets[thisPlayerID];
        socket.broadcast.emit('disconnected', player);
    });

});


function interval(func, wait, times){
    let interv = function(w,t){
        return function(){
            if(typeof t === "undefined" || t-- > 0){
                setTimeout(interv, w);
                try{
                    func.call(null);
                } catch (e) {
                    t = 0;
                    throw e.toSTring();
                }
            }
        };
    } (wait, times);

    setTimeout(interv, wait);
}
