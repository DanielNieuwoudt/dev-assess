interface TodoItemError {
    Title: string;
    Type: string;
    Detail: string;
    Status: number;
    Errors: { [key: string]: string[] };
    TraceId: string;
}