import path from "path";
import jestOpenAPI from "jest-openapi";

export function loadApiSpec(specPath :string): void {
    const apiDefinitionsPath = path.resolve(
        __dirname,
        `../../../specs/${specPath}` 
    );

    jestOpenAPI(apiDefinitionsPath);
}
