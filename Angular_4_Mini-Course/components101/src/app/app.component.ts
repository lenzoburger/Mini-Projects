import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `<h1>Project: Agular 4 101</h1>
<p>
</p>
<h2>Templates</h2>

<ul><b>Customer Details</b>
    <li>{{customerObj.name}}</li>
    <li>{{customerObj.age}}</li>
    <li>{{customerObj.country}}</li>
</ul>


<ul><b>Shopping List</b>
    <li *ngFor="let items of myShoppingList">{{items}}</li>
</ul>

  <h3 *ngIf="beNice; else otherTmpl">You're Awesome</h3>
  <ng-template #otherTmpl>You're Ok</ng-template>

<p></p>
  <div *ngIf="isRaining; then rainTmpl else sunTmpl;"></div>
    <ng-template #rainTmpl>It's raining</ng-template>
    <ng-template #sunTmpl>It's Sunny</ng-template>

<p></p>
<h2>Property Binding</h2>
<img src="{{ angularLogo }}">

<img *ngFor="let images of otherLogos" src="{{ images }}">

<img [src]="ASRockTuning">

<img bind-src="kodiBlack">
<p></p>
<button [disabled]="disableButton">Disabled Button</button>
<p></p>
<button [disabled]="!disableButton">Enabled Button</button>


<p></p>
<h2>Event Binding</h2>
`
  ,
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  customerObj = {
    name: 'lencho',
    age: 190,
    country: 'MarsOpia'
  }
  myShoppingList = ['fish','banana','mylk','veges']
  beNice = false;
  isRaining = false;

  angularLogo = '../assets/images/angularLogo.ico'

  otherLogos = ['../assets/images/kodiWhite.png', '../assets/images/ASRockTuningAlt.png']

  ASRockTuning = '../assets/images/ASRockTuning.png'

  kodiBlack = '../assets/images/kodiBlack.png'

  disableButton = true;
}
