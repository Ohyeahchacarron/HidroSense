import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, HttpClientModule],
  templateUrl: './registro.component.html',
})
export class RegistroComponent {
  form = this.fb.group({
    nombre: ['', Validators.required],
    apellidoPaterno: ['', Validators.required],
    apellidoMaterno: ['', Validators.required],
    edad: [null, [Validators.required, Validators.min(1)]],
    pais: ['', Validators.required],
    correo: ['', [Validators.required, Validators.email]],
    contrasenia: ['', Validators.required],
    telefono: ['', Validators.required],
  });

  paises = [/* lista como antes */];

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) {}

  onSubmit() {
    if (this.form.invalid) return;

    this.auth.registrarUsuario(this.form.value).subscribe({
      next: res => {
        console.log('Registro exitoso:', res);
        this.router.navigate(['/login']);
      },
      error: err => {
        console.error('Error al registrar:', err);
        alert(err.error?.message || 'Error desconocido');
      }
    });
  }
}
