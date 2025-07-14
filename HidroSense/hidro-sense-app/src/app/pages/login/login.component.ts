import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  form = this.fb.group({
    correo: ['', [Validators.required, Validators.email]],
    contrasenia: ['', Validators.required],
  });

  constructor(private fb: FormBuilder) { }

  onSubmit() {
    if (this.form.invalid) return;

    const datos = this.form.value;
    console.log('Login:', datos);
    // Aquí se llamaría al servicio para hacer login
  }
}
