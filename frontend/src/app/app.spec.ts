import { ComponentFixture, TestBed } from '@angular/core/testing';
import { App } from './app';

describe('App', () => {
  let fixture: ComponentFixture<App>;
  let app: App;
  let html: HTMLElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
    }).compileComponents();

    fixture = TestBed.createComponent(App);
    app = fixture.componentInstance;
    await fixture.whenStable();
    html = fixture.nativeElement as HTMLElement;
  });

  describe('ui', () => {
    it('should render title', () => {
      expect(html.querySelector('h1')?.textContent).toContain('URL Shortener');
    });
  });
});
