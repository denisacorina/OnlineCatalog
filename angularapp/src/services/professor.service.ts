import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Professor } from '../interfaces/professor.model';

@Injectable({
  providedIn: 'root'
})
export class ProfessorService {

  baseUrl = "https://localhost:7273/api/Professor/";


  constructor(private http: HttpClient) { }

  getProfessors() {
    return this.http.get<Professor[]>(`${this.baseUrl}GetAllProfessors`);
  }

  getProfessorById(professorId: string) {
    return this.http.get<Professor>(`${this.baseUrl}GetProfessorById/${professorId}`);
  }

  assignProfessorToCourse(professorId: string, courseId: number) {
    const body = {
      professorId: professorId,
      courseId: courseId
    };
    return this.http.post(`${this.baseUrl}AssignProfessorToCourse/${professorId}/${courseId}`, body)
  }

  addGrade(professorId: string, studentId: string, courseId: number, value: number) {
    const body = {
      professorId: professorId,
      studentId: studentId,
      courseId: courseId,
      value: value
    };
    return this.http.post(`${this.baseUrl}AddGrade/${courseId}/${studentId}/${professorId}/${value}`, body)
  }

}
