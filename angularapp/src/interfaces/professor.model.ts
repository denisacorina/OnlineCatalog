export interface Professor {
  professorId: string;
  firstName: string;
  lastName: string;
  courses?: Course[];
  grades?: Grade[];
}

export interface Student {
  studentId: string;
  firstName: string;
  lastName: string;
  courses?: Course[];
  grades?: Grade[];
}

export interface Course {
  courseId: number;
  courseName: string;
  semester: number;
  students?: Student[];
  professors?: Professor[];
  grades?: Grade[];
}

export interface Grade {
  gradeId: number;
  value: number;
  course: Course;
  student: Student;
  professor: Professor;
}
