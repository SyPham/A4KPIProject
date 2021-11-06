import { DataService } from 'src/app/_core/_service/data.service';
import {
  AfterViewInit,
  Directive,
  ElementRef,
  HostBinding,
  Input,
  OnDestroy,
  OnInit,
  Renderer2,
} from '@angular/core';
import { fromEvent, Subject, Subscription } from 'rxjs';
import { debounceTime, throttleTime } from 'rxjs/operators';
import { Custom } from '../_model/contribution';


@Directive({
  selector: '[tdHeight]',
  outputs: [
		"tdHeight",
	]
})
export class TDHeightDirective implements AfterViewInit, OnDestroy , OnInit {
  @Input()
  tdHeight: string;
  data: Array<Custom> = [];
  subscription: Subscription[] = [];
  subject = new Subject<string>();
  constructor(
    private renderer: Renderer2,
    private dataService: DataService,
    private elementRef: ElementRef
    ) {

  }

  ngAfterViewInit() {
  }

  ngOnInit(): void {
    setTimeout(() => {
      this.contentHeight(this.elementRef.nativeElement, this.tdHeight);
    }, 500);
  }


  ngOnDestroy(){
    this.subscription.forEach(item => item.unsubscribe());
  }

  contentHeight(parent: HTMLElement, className) {
    var height = parent.offsetHeight;
    var month = className;
    var data = new Custom();
    data.value = height;
    data.month = month;
    this.dataService.changeMessageTD(data)

  }

}
