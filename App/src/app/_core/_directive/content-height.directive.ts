import { Directive, ElementRef } from '@angular/core';

@Directive({
  selector: '[contentHeight]',
})
export class ContentHeightDirective {
  constructor(private el: ElementRef) {}

  ngOnInit() {
    const nativeElement = this.el.nativeElement;
    console.log(nativeElement);
    console.log(nativeElement.scrollHeight);
    nativeElement.style.height = 'auto';
  }
}
