import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { User } from '../models/user';
import { Injectable } from "@angular/core";
import { UserService } from "../services/user.service";
import { AlertifyService } from "../services/alertify.service";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";
@Injectable()
export class ListsResolver implements Resolve<User[]> {

    pageNumber = 1;
    pageSize = 5;
    likesParam = 'Likers';
    constructor(private userService: UserService, private router: Router,
        private alertify: AlertifyService) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<User[]> {
        return this.userService.getUsers(this.pageNumber, this.pageSize, null, this.likesParam).pipe(
            catchError(() => {
                this.alertify.error('Problem retrieving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }

}
