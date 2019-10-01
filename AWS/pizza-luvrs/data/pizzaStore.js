const Sequelize = require('sequelize');

const database = 'pizza_luvrs';
const host = 'pizza-db.cgtkw7rxytaz.ap-southeast-2.rds.amazonaws.com';
username = 'postgres';
password = 'tigerspike123';

pgClient = new Sequelize(
    database,
    username,
    password,{
        host: host,
        dialect: 'postgres'
    }
)

const Pizza = pgClient.define('pizza',{
    id:{
        type: Sequelize.STRING,
        primaryKey: true,
    },
    name:{
        type: Sequelize.STRING,
    },
    toppings:{
        type: Sequelize.STRING,
    },
    img:{
        type: Sequelize.STRING,
    },
    username:{
        type: Sequelize.STRING,
    },
    created:{
        type: Sequelize.BIGINT,
    },
})

Pizza.sync().then(() => {
    console.log('postgres connection ready')
})

module.exports = Pizza;