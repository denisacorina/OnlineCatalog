import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Student } from '../interfaces/professor.model';


@Injectable({
  providedIn: 'root'
})

export class StudentService {

  baseUrl = "https://localhost:7273/api/Student/";


  constructor(private http: HttpClient) { }

  getStudents() {
    return this.http.get<Student[]>(`${this.baseUrl}GetAllStudents`);
  }

  subscribeStudentToCourse(studentId: string, courseId: number) {
    const body = {
      studentId: studentId,
      courseId: courseId
    };
    return this.http.post(`${this.baseUrl}SubscribeToCourses/${studentId}/${courseId}`, body)
  }

}
