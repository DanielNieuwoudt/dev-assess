const express = require('express');
const path = require('path');
const fs = require('fs');

const app = express();

// Log all incoming requests
app.use((req, res, next) => {
    console.log(`Incoming request: ${req.method} ${req.url}`);
    next();
});

// Serve all static files except index.html
app.use(express.static(path.join(__dirname, 'build'), {
    index: false
}));

// Endpoint to serve environment variables as a JavaScript file
app.get('/env.js', (req, res) => {
    console.log('Serving env.js');
    res.setHeader('Content-Type', 'application/javascript');
    res.send(`window.env = ${JSON.stringify(process.env)};`);
});

// Serve index.html and inject env.js script tag
app.get('*', (req, res) => {
    console.log('Catch-all route triggered for', req.url);
    const indexFile = path.resolve(__dirname, 'build', 'index.html');
    console.log('Reading index.html from', indexFile);
    fs.readFile(indexFile, 'utf8', (err, data) => {
        if (err) {
            console.error('Error reading index.html:', err);
            return res.status(500).send('An error occurred');
        }

        console.log('Read index.html successfully');
        const scriptTag = '<script src="/env.js"></script>';
        if (!data.includes(scriptTag)) {
            data = data.replace('<head>', `<head>${scriptTag}`);
            console.log('Injected env.js script tag into index.html');
        } else {
            console.log('env.js script tag already present in index.html');
        }

        res.send(data);
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`Server is listening on port ${PORT}`);
});
