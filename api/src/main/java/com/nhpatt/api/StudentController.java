package com.nhpatt.api;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.support.ServletUriComponentsBuilder;

import java.util.ArrayList;
import java.util.List;
import java.util.NoSuchElementException;
import java.util.Optional;

@RestController
@RequestMapping("/students")
public class StudentController {

    List<Student> students = new ArrayList<>(List.of(
            new Student(0, "Javier", "Gamarra"),
            new Student(1, "Juan", "Sanz")));

    @GetMapping
    public List<Student> getStudent() {
        return students;
    }

    @Operation(summary = "Get an student by its id")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Found the student",
                    content = {@Content(mediaType = "application/json",
                            schema = @Schema(implementation = Student.class))})})
    @GetMapping("/{id}")
    @Tag(name = "students")
    public Student getStudent(@PathVariable int id) {
        return findStudent(id).orElseThrow(NoSuchElementException::new);
    }

    @DeleteMapping("/{id}")
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void deleteStudent(@PathVariable int id) {
        this.students = new ArrayList<>(students.stream().filter(student -> student.id() != id).toList());
    }

    @PostMapping
    public ResponseEntity<Student> createStudent(@RequestBody Student student) {
        this.students.add(student);
        return ResponseEntity.created(ServletUriComponentsBuilder.fromCurrentRequestUri().build().toUri()).body(student);
    }

    @PutMapping("/{id}")
    public Student replaceStudent(@PathVariable int id, @RequestBody Student newStudent) {
        this.students = new ArrayList<>(students.stream().filter(student -> student.id() != id).toList());
        this.students.add(newStudent);
        return newStudent;
    }

    @PatchMapping("/{id}")
    public Student updateStudent(@PathVariable int id, @RequestBody Student newStudent) {
        Optional<Student> maybeStudent = this.findStudent(newStudent.id());

        if (maybeStudent.isPresent()) {
            Student student = maybeStudent.get();
            Student studentUpdated = new Student(newStudent.id(), newStudent.name() == null ? student.name() : newStudent.name(), newStudent.surname() == null ? student.surname() : newStudent.surname());
            this.students = new ArrayList<>(students.stream().filter(studentFilter -> studentFilter.id() != id).toList());
            this.students.add(studentUpdated);
            return studentUpdated;
        }

        return newStudent;
    }

    private Optional<Student> findStudent(int id) {
        return students.stream().filter(student -> student.id() == id).findFirst();
    }

}
