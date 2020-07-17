import faker, { random } from 'faker';
import moment from 'moment';
import { randomEnumElement, setArrayTypes, genericSorter, sortGeneric } from './HelperFunctions';
import { v4 as uuidv4 } from 'uuid';
import { renderMetric } from './ValueRenderers';

const dateFormat = "MMMM Do, YYYY";

export enum YesOrNo {
  Yes = "Yes",
  No = "No"
}

export enum Gender {
  Male = "Male",
  Female = "Female"
}

export enum Race {
  AIAN = "American Indian or Alaska Native",
  Asian = "Asian",
  BAA = "Black or African American",
  NHOPI = "Native Hawaiian or Other Pacific Islander",
  White = "White"
}

export enum StudentIndicatorType {
  Other = "Other",
  Program = "Program"
}

export enum Relation {
  Mother = "Mother",
  Father = "Father"
}

export enum GoalMetric {
  MetGoal = "Met Goal",
  BelowGoal = "Below Goal",
  Caution = "Caution"
}

export enum ImprovementMetric {
  Better = "Getting Better",
  Worse = "Getting Worse",
  Same = "Same"
}

export enum MetricState {
  Good = "Good",
  Bad = "Bad",
  NA = ""
}

export enum MetricTrendDirection {
  Worse = -1,
  Same = 0,
  Better = 1
}

export enum Program {
  ELL = "English Language Learner (ELL)",
  GiftedAndTalented = "Gifted and Talented",
  Section504 = "Section 504",
  SpecialEducation = "Special Education",
  TitleIPartA = "Title I Part A"
}

export enum OtherIdentifier {
  AtRisk = "At Risk",
  EconomicallyDisadvantaged = "Economically Disadvantaged",
  FreeorReducedPricedLunchEligible = "Free or Reduced Priced Lunch Eligible",
  Homeless = "Homeless",
  Immigrant = "Immigrant",
  LimitedEnglishProficiency = "Limited English Proficiency",
  LimitedEnglishProficiencyMonitored1 = "Limited English Proficiency Monitored 1",
  LimitedEnglishProficiencyMonitored2 = "Limited English Proficiency Monitored 2",
  LimitedEnglishProficiencyMonitored3 = "Limited English Proficiency Monitored 3",
  LimitedEnglishProficiencyMonitored4 = "Limited English Proficiency Monitored 4",
  Migrant = "Migrant",
  OverAge = "Over Age"
}

export interface IAddress {
  cityStateZip(): string | null;

  addressLine1?: string | null;
  addressLine2?: string | null;
  addressLine3?: string | null;
  city?: string | null;
  state?: string | null;
  zipCode?: string | null;
}

export interface IFakeable {
  fakeData(): void;
}

export interface ICopyable {
  copy(obj: any): void;
}

export class MetricValue implements IFakeable {
  constructor(isPercent = false, hasImprovement=true) {
    this.isPercent = isPercent;
    this.hasImprovement = hasImprovement;
  }

  fakeData(): void {
    this.value = this.isPercent 
      ? faker.random.number({min: 0, max: 1, precision: 0.001}) 
      : faker.random.number(100);
    this.goal = randomEnumElement(GoalMetric);
    this.improvement = this.hasImprovement ? randomEnumElement(ImprovementMetric) : null;
  }

  readonly isPercent: boolean;
  readonly hasImprovement: boolean;
  value?: number | null;
  goal?: GoalMetric;
  improvement?: ImprovementMetric;
}

export class Metric {
  private _name?: string;
  private _id?: number;

  get name(): string | undefined {
    return this._name;
  }

  get id(): number | undefined {
    return this._id;
  }

  set id(newId: number | undefined) {
    this._id = newId;
    this.dataIndex = newId?.toString();
  }

  set name(newName: string | undefined) {
    this._name = newName;
    this.title = newName;
  }

  title?: string;
  dataIndex?: string | string[];
  parentId?: number;
  parentName?: string;
  state?: MetricState | null;
  trendDirection?: MetricTrendDirection | null;
  value?: string | null;
}

export class MetricNode extends Metric {
  constructor() {
    super();
    this.show = true;
    this.children = [];
    this.key = uuidv4();
  }

  show: boolean;
  key: string;
  children: MetricNode[];
  [x: string]: any;

  sortDataIndex(a:any, b:any) {
    return sortGeneric(a, b, this.dataIndex || "");
  }
}

export class StudentMetrics {
  constructor() {
    this.key = uuidv4();
  }

  fullName?: string | null;
  gradeLevel?: string | null;
  metrics?: Metric[] | null;
  schoolCategory?: string | null;
  schoolName?: string | null;
  studentUsi?: string | null;
  key: string;
  [x: string]: any
}

function getFakeMetricValue(isPercent=false, hasImprovement=true): MetricValue {
  var val = new MetricValue(isPercent, hasImprovement);
  val.fakeData();
  return val;
}

export class Contact implements IAddress {
  cityStateZip(): string | null {
    return this.city || this.state || this.zipCode ? 
      `${this.city}${this.city ? "," : ""} ${this.state} ${this.zipCode}`
      : null;
  }

  addressLine1?: string | null;
  addressLine2?: string | null;
  addressLine3?: string | null;
  city?: string | null;
  state?: string | null;
  zipCode?: string | null;
  telephoneNumber?: string | null;
}

export function generateFakeables<T extends IFakeable>(obj: new () => T, min = 1, max?: number) : Array<T> {
  max = max || min;
  const arrayLength = faker.random.number({min, max});
  return Array.from({length: arrayLength}).map(() => 
    { 
      var faked = new obj(); 
      faked.fakeData(); 
      return faked; 
    }
  )
}

export class StudentParentInformation extends Contact implements IFakeable {
  fakeData(): void {
    this.parentUsi = faker.random.number();
    this.studentUsi = faker.random.number();
    this.fullName = faker.name.findName();
    this.relation = randomEnumElement(Relation);
    this.addressLine1 = faker.address.streetAddress();
    this.addressLine2 = faker.address.secondaryAddress();
    this.addressLine3 = faker.address.secondaryAddress();
    this.city = faker.address.city();
    this.state = faker.address.stateAbbr();
    this.zipCode = faker.address.zipCode();
    this.telephoneNumber = faker.phone.phoneNumberFormat();
    this.workTelephoneNumber = faker.phone.phoneNumberFormat();
    this.emailAddress = faker.internet.email();
    this.primaryContact = faker.random.arrayElement([true, false]);
    this.livesWith = faker.random.arrayElement([true, false]);
  }

  parentUsi?: number | null;
  studentUsi?: number | null;
  fullName?: string | null;
  relation?: string | null;
  workTelephoneNumber?: string | null;
  emailAddress?: string | null;
  primaryContact?: boolean;
  livesWith?: boolean;
}

export class StudentIndicator implements IFakeable {
  fakeData(): void {
    this.studentUsi = faker.random.number();
    this.educationOrganizationId = faker.random.number();
    this.type = randomEnumElement(StudentIndicatorType);
    this.name = this.type === StudentIndicatorType.Program ? randomEnumElement(Program): randomEnumElement(OtherIdentifier);
    this.status = faker.random.arrayElement([true, false]);
    this.displayOrder = faker.random.number({min: 0, max: 100});
  }

  studentUsi?: number | null;
  educationOrganizationId?: number | null;
  type?: string | null;
  parentName?: string | null;
  name?: string | null;
  status?: boolean;
  displayOrder?: number | null;
}

export class StudentSchoolInformation implements IFakeable {
  fakeData(): void {
    this.studentUsi = faker.random.number();
    this.schoolId = faker.random.number();
    this.gradeLevel = faker.random.number(12).toString();
    this.homeroom = faker.random.word();
    this.lateEnrollment = randomEnumElement(YesOrNo);
    this.incompleteTranscript = randomEnumElement(YesOrNo);
    this.dateOfEntry = moment(faker.date.past(10)).format(dateFormat);
    this.dateOfWithdrawal = moment(faker.date.past(10)).format(dateFormat);
    this.withdrawalCode = faker.random.arrayElement([faker.random.alphaNumeric(), null])
    this.withdrawalDescription = faker.random.words();
    this.graduationPlan = faker.random.word();
    this.expectedGraduationYear = moment(faker.date.future(10)).format(dateFormat);
  }

  studentUsi?: number | null;
  schoolId?: number | null;
  gradeLevel?: string | null;
  homeroom?: string | null;
  lateEnrollment?: string | null;
  incompleteTranscript?: string | null;
  dateOfEntry?: string | null;
  dateOfWithdrawal?: string | null;
  withdrawalCode?: string | null;
  withdrawalDescription?: string | null;
  graduationPlan?: string | null;
  expectedGraduationYear?: string | null;
  feederSchools?: string | null;
}

export class StudentModel extends Contact implements IFakeable {
  constructor() {
    super();
    this.studentIndicators = [];
    this.studentParentInformation = [];
    this.studentSchoolInformation = [];
  }

  fakeData(): void {
    this.studentUsi = faker.random.number();
    this.fullName = faker.name.findName();
    this.firstName = faker.name.firstName();
    this.middleName = faker.name.firstName();
    this.lastSurname = faker.name.lastName();
    this.addressLine1 = faker.address.streetAddress();
    this.addressLine2 = faker.address.secondaryAddress();
    this.addressLine3 = faker.address.secondaryAddress();
    this.city = faker.address.city();
    this.state = faker.address.stateAbbr();
    this.zipCode = faker.address.zipCode();
    this.telephoneNumber = faker.phone.phoneNumberFormat();
    this.emailAddress = faker.internet.email();
    this.dateOfBirth = moment(faker.date.past(20)).format(dateFormat);
    this.placeOfBirth = `${faker.address.city()}, ${faker.address.stateAbbr()}`;
    this.currentAge = faker.random.number(20);
    this.cohortYear = faker.random.number({'min': 1980, 'max': 2020});
    this.gender = faker.random.arrayElement(Object.keys(Gender));
    this.oldEthnicity = faker.random.word();
    this.hispanicLatinoEthnicity = randomEnumElement(YesOrNo);
    this.race = randomEnumElement(Race);
    this.homeLanguage = faker.random.word();
    this.language = faker.random.word();
    this.parentMilitary = randomEnumElement(YesOrNo);
    this.singleParentPregnantTeen = randomEnumElement(YesOrNo);
    this.profileThumbnail = faker.image.avatar();
    this.studentIndicators = generateFakeables(StudentIndicator, 1, 7);
    this.studentSchoolInformation = generateFakeables(StudentSchoolInformation);
    this.studentParentInformation = generateFakeables(StudentParentInformation, 1, 2);
  }

  indicatorsOfType(type: StudentIndicatorType): StudentIndicator[] {
    return this.studentIndicators !== null ? this.studentIndicators.filter(val => val.status && val.type === type) : [];
  }

  otherIndicators(): StudentIndicator[] {
    return this.indicatorsOfType(StudentIndicatorType.Other);
  }

  hasOtherIndicators(): boolean {
    return this.otherIndicators().length > 0;
  }

  programs(): StudentIndicator[] {
    return this.indicatorsOfType(StudentIndicatorType.Program);
  }

  hasPrograms(): boolean {
    return this.programs().length > 0;
  }

  copy(otherObj: any): void {
    Object.assign(this, otherObj);
    this.studentParentInformation = setArrayTypes(StudentParentInformation, otherObj.studentParentInformation);
    this.studentSchoolInformation = setArrayTypes(StudentSchoolInformation, otherObj.studentSchoolInformation);
    this.studentIndicators = setArrayTypes(StudentIndicator, otherObj.studentIndicators);
  }

  studentUsi?: number | null;
  fullName?: string | null;
  firstName?: string | null;
  middleName?: string | null;
  lastSurname?: string | null;
  emailAddress?: string | null;
  dateOfBirth?: string | null;
  placeOfBirth?: string | null;
  currentAge?: number | null;
  cohortYear?: number | null;
  gender?: string | null;
  oldEthnicity?: string | null;
  hispanicLatinoEthnicity?: string | null;
  race?: string | null;
  homeLanguage?: string | null;
  language?: string | null;
  parentMilitary?: string | null;
  singleParentPregnantTeen?: string | null;
  profileThumbnail?: string | null;
  studentIndicators: StudentIndicator[] | null;
  studentSchoolInformation: StudentSchoolInformation[] | null;
  studentParentInformation: StudentParentInformation[] | null;
}

export class StudentRowModel implements IFakeable {
  fakeData(): void {
    this.studentUsi = faker.random.number();
    this.name = faker.name.findName();
    this.grade = faker.random.number(12);
    this.attendance = {
      twentyDayAttendance: getFakeMetricValue(true),
      fortyDayAttendance: getFakeMetricValue(true),
      ytdAttendance: getFakeMetricValue(true, false),
      absences: getFakeMetricValue(false, false),
      unexcusedAbsences: getFakeMetricValue(false, false),
      twentyDayAbsences: getFakeMetricValue(true),
      fortyDayAbsences: getFakeMetricValue(true),
      ytdAbsences: getFakeMetricValue(true, false),
      stateOffenses: getFakeMetricValue(false, false),
      nonStateOffenses: getFakeMetricValue(false, false),
      ytdTardyRate: getFakeMetricValue(true, false),
    }
    this.grades = {
      failing: getFakeMetricValue(false, false),
      belowProficiency: getFakeMetricValue(),
      droppingBelowProficiency: getFakeMetricValue(),
      creditAccumulation: getFakeMetricValue(false, false),
    }
    this.careerReadiness = {
      actAspireTaken: faker.random.arrayElement([true, false]),
      actTaken: faker.random.arrayElement([true, false])
    }
  }

  studentUsi?: number | null;
  name?: string | null;
  grade?: number | null;
  attendance?: {
    twentyDayAttendance?: MetricValue;
    fortyDayAttendance?: MetricValue;
    ytdAttendance?: MetricValue;
    absences?: MetricValue;
    unexcusedAbsences?: MetricValue;
    twentyDayAbsences?: MetricValue;
    fortyDayAbsences?: MetricValue;
    ytdAbsences?: MetricValue;
    stateOffenses?: MetricValue;
    nonStateOffenses?: MetricValue;
    ytdTardyRate?: MetricValue;
  };
  grades?: {
    failing?: MetricValue;
    belowProficiency?: MetricValue;
    droppingBelowProficiency?: MetricValue;
    creditAccumulation?: MetricValue;
  };
  careerReadiness?: {
    actAspireTaken?: boolean;
    actTaken?: boolean;
  };
}