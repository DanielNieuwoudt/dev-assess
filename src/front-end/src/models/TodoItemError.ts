interface TodoItemError {
    Title: string;
    Type: string;
    Status: number;
    Errors: { [key: string]: string[] };
    TraceId: string;
}