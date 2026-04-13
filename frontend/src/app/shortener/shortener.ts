import { httpResource } from '@angular/common/http';
import { Component, effect, signal } from '@angular/core';
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
    const request = this.#shortenedUrlRequest();

    if (!request.sourceUrl)
      return;

    return ({
      url: environment.shortenerApiBaseUrl + '/urls',
      method: 'POST',
      body: {
        'url': request.sourceUrl
      }
    })
  });

  #shortenedUrlRequest = signal<ShortenerRequest>({ sourceUrl: '' });

  #model = signal<ShortenerRequest>({ sourceUrl: '' });

  form = form(this.#model, (schemaPath) => {
    required(schemaPath.sourceUrl, { message: 'URL required' })
  });

  constructor() {
    effect(() => {
      if (this.shortenedUrl.hasValue()) {
        this.form.sourceUrl().value.set('');
        this.form().reset();
      }
    });
  }

  onSubmit(event: Event) {
    event.preventDefault();
    this.#shortenedUrlRequest.set(this.form().value());
  }
}
