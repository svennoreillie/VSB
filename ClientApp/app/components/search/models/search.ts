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

    public pillar: string;

    public stateInitiated: boolean;
    public stateCompleted: boolean;
    public stateCompletedDate: string;
    public stateRejected: boolean;
    public stateRejectedDate: string;

    public federation: number;

    constructor() {
        super();

        let firstOfTheMonth = new Date();
        firstOfTheMonth.setDate(1);
        this.stateCompletedDate = firstOfTheMonth.toISOString();
        this.stateRejectedDate = firstOfTheMonth.toISOString();
    }
}
