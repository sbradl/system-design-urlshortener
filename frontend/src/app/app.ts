import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Shortener } from './shortener/shortener';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Shortener],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
}
