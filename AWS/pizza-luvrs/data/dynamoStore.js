const AWS = require('aws-sdk');

AWS.config.update({ region: 'ap-southeast-2' })

const dynamodb = new AWS.DynamoDB.DocumentClient();

async function putItem(table, item) {
    return new Promise((resolve, reject) => {
        const params = {
            TableName: table,
            Item: item
        }

        dynamodb.put(params, (err, data) => {
            if (err) {
                reject(err)
            } else {
                resolve(data)
            }
        })
    })
}

async function getAllItems(table) {
    return new Promise((resolve, reject) => {
        const params = {
            TableName: table,
        }
        console.log('getting all items')
        
        dynamodb.scan(params, (err, data) => {
            if(err){
                console.log('Failed')
                reject(err)
            }else{
                console.log('Passed')
                console.log(data.Items)
                resolve(data.Items)
            }
        })
    })
}

async function getItem(table, idKey, id) {
    return new Promise((resolve, reject) => {
        const params = {
            TableName: table,
            Key: {
                [idKey]: id
            }
        }

        dynamodb.get(params, (err, data) => {
            if(err){
                reject(err)
            }else{
                resolve(data.Item)
            }
        })
    })
}

module.exports = {
    putItem,
    getAllItems,
    getItem
}