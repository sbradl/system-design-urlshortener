import { httpResource } from '@angular/common/http';
import { Component, signal } from '@angular/core';
import { form, FormField, required } from '@angular/forms/signals';
import { environment } from '../../environments/environment';

interface ShortenerRequest {
  sourceUrl: string;
}

interface ShortenerResponse {
  url: string;
}

@Component({
  selector: 'app-shortener',
  imports: [FormField],
  templateUrl: './shortener.html',
  styleUrl: './shortener.css',
})
export class Shortener {
  shortenedUrl = httpResource<ShortenerResponse>(() => {
    const trigger = this.#sourceUrlEntered();
    const sourceUrl = this.form.sourceUrl().value();

    if (trigger === 0)
      return;

    return ({
      url: environment.shortenerApiBaseUrl + '/shorten',
      method: 'POST',
      body: {
        'url': sourceUrl
      }
    })
  });

  #model = signal<ShortenerRequest>({ sourceUrl: '' });
  #sourceUrlEntered = signal(0);

  form = form(this.#model, (schemaPath) => {
    required(schemaPath.sourceUrl, { message: 'URL required' })
  });

  onSubmit(event: Event) {
    event.preventDefault();
    this.#sourceUrlEntered.update(v => v + 1);
  }
}
