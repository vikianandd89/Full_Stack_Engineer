import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { RouterModule } from '@angular/router';
import { Routes } from '@angular/router';
import { AddComponent } from './task-manager/add/add.component';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';
import { TaskManagerService } from './task-manager/providers/task-manager.service';
import { ViewComponent } from './task-manager/view/view.component';
import { SearchPipe } from './task-manager/providers/search.pipe';
import { EditComponent } from './task-manager/edit/edit.component';

const appRoutes: Routes = [
  { path: "", component: AddComponent },
  { path : "add", component: AddComponent, pathMatch : "full" },
  { path : "view", component: ViewComponent, pathMatch : "full" },
  { path : "edit", component: EditComponent, pathMatch : "full" }
]

@NgModule({
  declarations: [
    AppComponent,
    AddComponent,
    ViewComponent,
    SearchPipe,
    EditComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    FormsModule,
    RouterModule.forRoot(appRoutes)
  ],
  providers: [
    TaskManagerService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
