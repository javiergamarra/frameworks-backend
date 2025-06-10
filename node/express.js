const express = require('express');
const app = express();

const students = [{name: "javier", surname: "gamarra"}, {name: "jorge", surname: "sanz"},]

app.get('/students', (req, res) => {
    res.send(students);
});

app.get('/students/0', (req, res) => {
    res.send(students[0]);
});

app.delete('/students/0', (req, res) => {
    students.splice(0, 1)
    res.send(undefined)
});

const PORT = process.env.PORT || 3000;

app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
