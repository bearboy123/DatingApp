import { Component, OnInit, Input, EventEmitter,Output } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  model: any = {};
  @Output() cancelRegister = new EventEmitter();
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  register() {
    this.authService.register(this.model).subscribe(() => {
      console.log('Registered successfully');
    }, error => {
      console.log('Error while registering');
    } );
  }

  cancel() {
    this.cancelRegister.emit(false);
    console.log('cancelled');
  }
}
