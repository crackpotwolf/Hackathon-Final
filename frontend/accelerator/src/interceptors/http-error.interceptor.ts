import {Injectable} from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import {Observable, of, throwError} from 'rxjs';
import {catchError, map} from "rxjs/operators";
import {MessageService} from "primeng/api";

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(private messageService: MessageService) {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request)
      .pipe(catchError(err => {
        if ([504, 502].indexOf(err.status) != -1) {
          this.messageService.add({
            severity: 'error',
            summary: 'Сервер не доступен',
            // detail: err.message
          });
        }
        return throwError(err);
      }));
  }
}


