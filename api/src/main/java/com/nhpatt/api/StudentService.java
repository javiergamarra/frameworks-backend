package com.nhpatt.api;

import org.modelmapper.ModelMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

@Service
public class StudentService {

    @Autowired
    private RestTemplate restTemplate;

    @Autowired
    private ModelMapper modelMapper;

    private List<Student> students = new ArrayList<>(List.of(
            new Student(0, "Javier", "Gamarra"),
            new Student(1, "Juan", "Sanz")));

    public List<StudentDTO> getAllStudents() {

        ResponseEntity<Object> forEntity = this.restTemplate.getForEntity("https://cdn.contentful.com/spaces/7bqz4c5fa32k/environments/master/entries/JeOVkhPsOUaAwJBNuyTyZ?access_token=gckuEehZljgwMhBdcwYzuOwu0lcC0qWBKXMJ3rwZs5Y", Object.class);

        return students.stream()
                .map(student -> modelMapper.map(student, StudentDTO.class))
                .collect(Collectors.toList());
    }

    public Optional<Student> getStudentById(int id) {
        return students.stream().filter(s -> s.getId() == id).findFirst();
    }

    public void deleteStudent(int id) {
        students = students.stream().filter(s -> s.getId() != id).toList();
    }

    public Student createStudent(Student student) {
        students.add(student);
        return student;
    }

    public Student replaceStudent(int id, Student newStudent) {
        deleteStudent(id);
        students.add(newStudent);
        return newStudent;
    }

    public Student updateStudent(int id, Student updatedData) {
        Optional<Student> maybeStudent = getStudentById(id);

        if (maybeStudent.isPresent()) {
            Student existing = maybeStudent.get();
            Student updated = new Student(
                    id,
                    updatedData.getName() != null ? updatedData.getName() : existing.getName(),
                    updatedData.getSurname() != null ? updatedData.getSurname() : existing.getSurname()
            );
            deleteStudent(id);
            students.add(updated);
            return updated;
        }

        return updatedData;
    }
}