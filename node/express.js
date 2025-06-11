const express = require('express');
const app = express();

app.use(express.json());

const Joi = require('joi');
const swaggerJsDoc = require('swagger-jsdoc');
const swaggerUi = require('swagger-ui-express');

const axios = require('axios');

const swaggerOptions = {
    definition: {
        openapi: '3.0.0',
        info: {
            title: 'User API',
            version: '1.0.0',
            description: 'User management API'
        },
    },
    apis: ['*.js']
};
const swaggerDocs = swaggerJsDoc(swaggerOptions);
app.use('/api-docs', swaggerUi.serve, swaggerUi.setup(swaggerDocs));


const studentSchema = Joi.object({
    name: Joi.string().alphanum().min(3).max(30).required(),
    birthYear: Joi.number().integer().min(1900).max(new Date().getFullYear()),
    surname: Joi.string(),
});

const validateUser = (req, res, next) => {
    const {error} = studentSchema.validate(req.body);
    if (error) {
        return res.status(400).json({
            message: 'Validation error',
            details: error.details.map(x => x.message)
        });
    }
    next();
};


let students = [{id: 0, name: "javier", surname: "gamarra"}, {id: 1, name: "jorge", surname: "sanz"},]

app.use((req, res, next) => {
    console.log('Middleware');
    next();
});


/**
 * @swagger
 * /students:
 *   get:
 *     summary: Returns a list of students
 *     responses:
 *       200:
 *         description: A list of students
 *         content:
 *           application/json:
 *             schema:
 *               type: array
 *               items:
 *                 type: object
 *                 properties:
 *                   id:
 *                     type: integer
 *                   name:
 *                     type: string
 */
app.get('/students', async (req, res) => {

    const response = await axios.get("https://cdn.contentful.com/spaces/7bqz4c5fa32k/environments/master/entries/JeOVkhPsOUaAwJBNuyTyZ?access_token=gckuEehZljgwMhBdcwYzuOwu0lcC0qWBKXMJ3rwZs5Y");
    console.error(response.data)

    res.send(students);
});

app.get('/students/:id', (req, res) => {
    res.send(students[req.params.id]);
});

app.delete('/students/:id', (req, res) => {
    students.splice(req.params.id, 1)
    res.send(undefined)
});

app.post('/students', validateUser, (req, res) => {
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
