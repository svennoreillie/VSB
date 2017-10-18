//This file is just for letting typescript think the json file is a module

declare module "*.json" {
    const value: any;
    export default value;
}