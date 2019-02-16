import { Component } from '@angular/core';
import { DataService } from './data.service';
import { trigger,state,style,transition,animate,keyframes } from '@angular/animations'

@Component({
  selector: 'app-root',
  template: `<h1>Project: Agular 4 101</h1>
<p>
</p>
<h2>1.Templates</h2>

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
<h2>2.Property Binding</h2>
<img src="{{ angularLogo }}">

<img *ngFor="let images of otherLogos" src="{{ images }}">

<img [src]="ASRockTuning">

<img bind-src="kodiBlack">
<p></p>
<button [disabled]="disableButton">Disabled Button</button>
<p></p>
<button [disabled]="!disableButton">Enabled Button</button>


<p></p>
<h2>3.Event Binding</h2>
<button (click)="myEvent($event)">Click Me</button>
<p></p>
<button (mouseenter)="myEvent($event)">Mouse Hover</button>

<p></p>
<h2 class="styleMe">4.Defining Style Sheets</h2>

<p></p>
<h2 >5.Class Binding</h2>

<h3 [class]="titleClass1">titleClass1</h3>

<h3 [class.blue-title]="titleClass2">titleClass2</h3>

<h3 [ngClass]="multiClasses">multiClass</h3>



<p></p>
<h2 >6.Style Binding</h2>

<h3 [style.color]="titleStyle1">style.color</h3>

<h3 [style.color]="titleStyle2 ? 'green' : 'pink'">style.color.Conditional</h3>

<h3 [ngStyle]="multiStyles">ngStyle</h3>


<p></p>
<h2 >7. Services</h2>
<p>{{someProperty}}</p>

<p></p>
<h2 >8. Animation</h2>
<p class="animated" [@myAwesomeAnimation]='state' (click)="animateMe()">I will animate</p>

<p class="animated" [@myKeyFrameAnimation]='stateKeyFrame' (click)="animateMeKeyFrame()">Keyframe animation</p>

`
  ,
  styles: [`

  h2{
    text-decoration:underline;
  }

  .styleMe{
    color: green;
    font-family: verdana;
    font-size: 25px;
    padding-top: 15px;
    padding-bottom: 45px;
    text-shadow: 3px 2px red;
  }

  .red-title{
    color: red;
  }

  .blue-title{
    color: blue;
  }

  .green-title{
    color: green;
  }

  .large-title{
    font-size: 4em;
  }

  .animated{
    width: 200px;
    background: lightgray;
    margin: 100px auto;
    text-align: center;
    padding: 20px;
    font-size: 1.5em;
  }
`],

  animations: [
    trigger('myAwesomeAnimation', [

      state('small', style({
        transform: 'scale(1)'
      })),
      state('large', style({
        transform: 'scale(1.2)'
      })),
      transition('small <=> large', animate('300ms ease-in', style({
        transform: 'translateY(40px)'
      })))
    ]),

    trigger('myKeyFrameAnimation', [

      state('small', style({
        transform: 'scale(1)'
      })),
      state('large', style({
        transform: 'scale(1.2)'
      })),
      transition('small <=> large', animate('600ms ease-in', keyframes([
        style({ opacity: 0, transform: 'translateY(-75%)', offset: 0 }),
        style({ opacity: 1, transform: 'translateY(35px)', offset: .5 }),
        style({ opacity: 1, transform: 'translateY(0)', offset: 1 })
      ])))
    ])


  ]
})
export class AppComponent {

  constructor(private dataService: DataService) {

  }

  state: string = 'small'

  stateKeyFrame: string = 'small'

  someProperty: string = '';

  ngOnInit() {
    console.log(this.dataService.cars)
    this.someProperty = this.dataService.myData();
  }

  animateMe() {
    this.state = (this.state === 'small' ? 'large' : 'small');
  }

  animateMeKeyFrame() {
    this.stateKeyFrame = (this.stateKeyFrame === 'small' ? 'large' : 'small');
  }
  
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

  myEvent(event) {
    console.log(event);
  }

  titleClass1 = 'red-title';

  titleClass2 = true;

  multiClasses = {
    'green-title': true,
    'large-title': true
  }

  titleStyle1 = 'red';

  titleStyle2 = true;

  multiStyles = {
    'color': 'brown',
    'text-decoration': 'line-through'
  }
}
