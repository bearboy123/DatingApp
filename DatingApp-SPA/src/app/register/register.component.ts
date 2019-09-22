import { Component, OnInit, Input, EventEmitter,Output } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { AlertifyService } from '../services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  model: any = {};
  @Output() cancelRegister = new EventEmitter();
  constructor(private authService: AuthService,private alertify:AlertifyService) { }

  ngOnInit() {
  }

  register() {
    this.authService.register(this.model).subscribe(() => {
      this.alertify.success('Registered successfully');
    }, error => {
      this.alertify.error(error);
    } );
  }

  cancel() {
    this.cancelRegister.emit(false);
    console.log('cancelled');
  }
}
