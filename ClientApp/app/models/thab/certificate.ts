export interface THABCertificate {
    beginDate: Date;
    certificateId: string;
    decisionDate: Date;
    endDate: Date;
    isMigrated: boolean;
    migrateDate: Date;
    referenceDate: Date;
    registrationDate: Date;
    state: string;
    terminationReason: string;
}