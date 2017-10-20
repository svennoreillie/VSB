export class SearchModel {
}

export class NameSearchModel extends SearchModel {
    public firstName?: string;
    public name: string;
}

export class MemberNrSearchModel extends SearchModel {
    public memberNr: number;
}

export class InszSearchModel extends SearchModel {
    public insz: string;
}