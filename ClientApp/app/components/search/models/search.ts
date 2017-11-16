export class SearchModel {
    public skip: number = 0;
    public limit: number = 100;
}

export class NameSearchModel extends SearchModel {
    public firstName?: string;
    public name: string;
}

export class MemberNrSearchModel extends SearchModel {
    public federation: number;
    public memberNr: number;
}

export class InszSearchModel extends SearchModel {
    public insz: string;
}

export class SiNumberSearchModel extends SearchModel {
    public siNumber: number;
}

export class StateSearchModel extends SearchModel {

    public ZVZ: boolean;
    public BOB: boolean;
    public THAB: boolean;

    public StateInitiated: boolean;
    public StateCompleted: boolean;
    public StateCompletedDate: string;
    public StateRejected: boolean;
    public StateRejectedDate: string;

    public Federation: number;

    constructor() {
        super();

        let firstOfTheMonth = new Date();
        firstOfTheMonth.setDate(1);
        this.StateCompletedDate = firstOfTheMonth.toISOString();
        this.StateRejectedDate = firstOfTheMonth.toISOString();
    }
}
