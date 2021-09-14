import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { RemovingEventArgs, UploaderComponent } from '@syncfusion/ej2-angular-inputs';

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html',
  styleUrls: ['./upload-file.component.scss']
})
export class UploadFileComponent implements OnInit {
  @ViewChild('defaultupload')
  public uploadObj: UploaderComponent;
  constructor( public activeModal: NgbActiveModal) { }

  public dropElement: HTMLElement = document.getElementsByClassName('control-fluid')[0] as HTMLElement;

  public path: Object = {
    saveUrl: 'https://ej2.syncfusion.com/services/api/uploadbox/Save',
    removeUrl: 'https://ej2.syncfusion.com/services/api/uploadbox/Remove'
};
  ngOnInit() {
  }
  public onFileRemove(args: RemovingEventArgs): void {
    args.postRawFile = false;
}
}
