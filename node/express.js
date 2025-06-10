const express = require('express');
const app = express();

app.use(express.json());

let students = [{id: 0, name: "javier", surname: "gamarra"}, {id: 1, name: "jorge", surname: "sanz"},]

app.get('/students', (req, res) => {
    res.send(students);
});

app.get('/students/:id', (req, res) => {
    res.send(students[req.params.id]);
});

app.delete('/students/:id', (req, res) => {
    students.splice(req.params.id, 1)
    res.send(undefined)
});

app.post('/students', (req, res) => {
    students.push(req.body)
    res.send(req.body);
});

app.patch('/students/:id', (req, res) => {
    const find = students.find(student => student.id == req.params.id);
    find.name = req.body.name ?? find.name;
    find.surname = req.body.surname ?? find.surname;
    res.send(find)
});

app.put('/students/0', (req, res) => {
    students = students.filter(student => student.id != req.params.id);
    students.push(req.body)
    res.send(req.body)
});

const PORT = process.env.PORT || 3000;

app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
