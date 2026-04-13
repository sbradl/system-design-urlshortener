import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Shortener } from './shortener';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting, TestRequest } from '@angular/common/http/testing';
import { environment } from '../../environments/environment';

describe('Shortener', () => {
  let component: Shortener;
  let fixture: ComponentFixture<Shortener>;
  let html: HTMLElement;

  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
      imports: [Shortener],
    }).compileComponents();

    httpMock = TestBed.inject(HttpTestingController);

    fixture = TestBed.createComponent(Shortener);
    component = fixture.componentInstance;
    await fixture.whenStable();
    html = fixture.nativeElement as HTMLElement;
  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('ui', () => {
    it('should render url input', () => {
      expect(urlInput()).not.toBeNull();
      expect(urlInput().getAttribute('aria-invalid')).toBe('false');
    });

    it('should mark url input as invalid when empty', () => {
      enterUrl('test');
      enterUrl('');
      expect(urlInput().getAttribute('aria-invalid')).toBe('true');
    })

    it('should render submit button', () => {
      expect(submitButton()).not.toBeNull();
    });

    it('should not display shortened-url initially', () => {
      expect(shortenedUrlElement()).toBeNull();
    });
  });

  describe('submit button state', () => {
    describe('given no url', () => {
      it('should disable submit button', () => {
        expect(submitButton().disabled).toBe(true);
      });
    });

    describe('given url is entered', () => {
      it('should enable submit button', async () => {
        enterUrl('url');
        await fixture.whenStable();

        expect(submitButton().disabled).toBe(false);
      });
    });
  });

  describe('shortening', () => {
    it('should display shortened url', async () => {
      const sourceUrl = 'http://some-long-url/some-random-stuff';
      const req = await shorten(sourceUrl);

      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual({ url: sourceUrl });

      req.flush({
        url: 'http://short.de/1234'
      });
      await fixture.whenStable();

      expect(shortenedUrl()).toBe('http://short.de/1234');
      expect(shortenedUrlElement()?.href).toBe('http://short.de/1234');
    });

    it('should display error when shortening failed', async () => {
      const sourceUrl = 'http://some-long-url/some-random-stuff';
      const req = await shorten(sourceUrl);

      req.flush('Failed!', { status: 500, statusText: 'Failed' });
      await fixture.whenStable();

      expect(shortenedUrl()).toBe('');
      expect(errorMessage()).toBe('Something went wrong');
    });

    it('should mark submit button as busy while shortening', async () => {
      const sourceUrl = 'http://some-long-url/some-random-stuff';
      const req = await shorten(sourceUrl);

      expect(submitButton().getAttribute('aria-busy')).toBe('true');
      req.flush({
        url: 'http://short.de/1234'
      });
      await fixture.whenStable();

      expect(submitButton().getAttribute('aria-busy')).toBe('false');
    });

    it('should clear entered url on success', async () => {
      const sourceUrl = 'http://some-long-url/some-random-stuff';
      const req = await shorten(sourceUrl);

      req.flush({
        url: 'http://short.de/1234'
      });
      await fixture.whenStable();

      expect(urlInput()?.textContent).toBe('');
      expect(urlInput().getAttribute('aria-invalid')).toBe('false');
    });
  });

  async function shorten(url: string): Promise<TestRequest> {
    enterUrl(url);

    let preventDefaultCalled = false;
    component.onSubmit(<Event>{ preventDefault: () => { preventDefaultCalled = true } });
    fixture.detectChanges();

    expect(preventDefaultCalled).toBe(true);

    return httpMock.expectOne(environment.shortenerApiBaseUrl + '/urls');
  }

  function urlInput(): HTMLInputElement {
    return <HTMLInputElement>(html.querySelector('input[name*="sourceUrl"]'));
  }

  function shortenedUrlElement(): HTMLAnchorElement | null {
    return html.querySelector('.shortened-url');
  }

  function shortenedUrl(): string {
    return shortenedUrlElement()?.textContent ?? '';
  }

  function errorMessage() {
    return html.querySelector('.error-message')?.textContent ?? '';
  }

  function enterUrl(url: string) {
    component.form.sourceUrl().value.set(url);
    component.form().markAsDirty();
    fixture.detectChanges()
  }

  function submitButton(): HTMLInputElement {
    return <HTMLInputElement>(html.querySelector('input[type="submit"]'));
  }
});
