import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Course, Professor, Student, Grade } from '../interfaces/professor.model';
import { ProfessorService } from '../services/professor.service';
import { StudentService } from '../services/student.service';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  professorId: string = '';
  courseId!: number;
  studentId!: string;
  value!: number;
  professors!: Professor[];
  courses!: Course[];
  students!: Student[];
  grades!: Grade[];

  professor!: Professor | null;



  constructor(private professorService: ProfessorService, private studentService: StudentService, private router: Router) {
    this.getProfessors();
    this.getStudents();

  }



  getProfessors() {
    this.professorService.getProfessors().subscribe({
      next: (result) => {
        this.professors = result;
        console.log(result);

      },
      error: error => console.error(error)
    });
  }

  assignProfessorToCourse() {
    this.professorService.assignProfessorToCourse(this.professorId, this.courseId)
      .subscribe(() => {
        console.log(`Professor assigned to course`);
      }, (error: any) => {
        console.error(`Error assigning professor to course: ${error.message}`);
      });
  }

  addGrade() {
    this.professorService.addGrade(this.professorId, this.studentId, this.courseId, this.value)
      .subscribe(
        response => {
          console.log(response);
          this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
            this.router.navigate(['']);
          });
        },
        error => console.log(error)
      );
  }

  getStudents() {
    this.studentService.getStudents().subscribe({
      next: (result) => {
        this.students = result;
        console.log(result);

      },
      error: error => console.error(error)
    });
  }

  subscribeStudentToCourse() {
    this.studentService.subscribeStudentToCourse(this.studentId, this.courseId)
      .subscribe(result => {
        console.log(result);
      });
  }
}


