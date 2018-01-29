export interface StepBaseComponent {
    activate(): void;
    isCompleted(): boolean;
    deactivate(): void
}  