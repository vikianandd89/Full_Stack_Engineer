import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Router,ActivatedRoute} from '@angular/router';

import { TaskItem } from '../../models/task-item';
import { TaskManagerService } from '../providers/task-manager.service';

@Component({
  templateUrl: './edit.component.html'
})
export class EditComponent implements OnInit {

    @ViewChild('showmodal') showmodal:ElementRef;
    taskDetails:TaskItem[]
    taskDetail:TaskItem;
    updateTaskId:number
    results:string
    showError:boolean = false;
    constructor(
        private taskManagerService:TaskManagerService,
        private route: ActivatedRoute,
        private router: Router
    ) { 
        this.taskDetail = new TaskItem();
        this.taskDetail.priority = 0;
    }

    ngOnInit() {
        this.route.queryParams.subscribe(params => {
        this.updateTaskId = params['id'];
    })

    this.taskManagerService.getAllTasks().subscribe(
      response=>this.taskDetails=response.filter(resElement => resElement.id !=  this.updateTaskId && !resElement.endTask));

      this.taskManagerService.getTask(this.updateTaskId)
        .subscribe(response => { this.taskDetail =  response; },
        error => {
            if(error.status < 200 || error.status > 300)
            {    
              this.showError = true;     
              this.results = JSON.parse(error._body);          
            }
        });
    }

    onUpdateTask() {
        this.taskManagerService.updateTask(this.taskDetail, this.taskDetail.id)
            .subscribe(response => {
                this.results = response;
                this.openModal();
            },
            error => {
                if(error.status < 200 || error.status > 300)
                this.results = JSON.parse(error._body);
                this.openModal();
            }
      );
  }

  onCancel() {
    this.router.navigate(['/view']);
  }

  openModal() {
    this.showmodal.nativeElement.click();
  }

  closeModal() {
    this.router.navigate(['/view']);
  }
}