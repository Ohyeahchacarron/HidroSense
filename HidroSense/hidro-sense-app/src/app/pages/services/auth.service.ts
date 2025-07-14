import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'http://localhost:7016/api/Usuarios'; // Ajusta si tu puerto o endpoint es diferente

  constructor(private http: HttpClient) { }

  registrarUsuario(datos: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/registro`, datos);
  }
}
