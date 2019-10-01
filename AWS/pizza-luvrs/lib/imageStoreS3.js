const AWS = require('aws-sdk');

const s3 = new AWS.S3();

module.exports.save = (name, data) => {

    const params = {
        Bucket: 'pizza-luvrs-bucket-lencho',
        Key: `pizzas/${name}.png`,
        Body: new Buffer(data, 'base64'),
        ContentEncoding: 'base64',
        ContentType: 'image/png'
    };

    s3.putObject(params, (err, data) => {
        if (err) console.log(err, err.stack); // an error occurred
        else     console.log(data);           // successful response
    });
};