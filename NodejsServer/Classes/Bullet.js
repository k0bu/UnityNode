const ServerObject = require('./ServerObjects.js');
const Vector2 = require('./Vector2.js')

module.exports = class Bullet extends ServerObject {
    constructor(){
        super();
        this.name = 'Bullet';
        this.direction = new Vector2();
        this.speed = 0.5;

    }

    onUpdate(){

        this.position.x += this.direction.x * this.speed;
        this.position.y += this.direction.y * this.speed;

        return false;
    }
}