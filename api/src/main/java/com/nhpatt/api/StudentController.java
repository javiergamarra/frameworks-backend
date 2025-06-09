package com.nhpatt.api;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

@RestController
public class StudentController {

    List<Student> students = List.of(
            new Student("Javier", "Gamarra"),
            new Student("Juan", "Sanz"));

    @GetMapping("/students")
    public List<Student> getStudent() {
        return students;
    }
}
