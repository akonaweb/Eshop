import { ReactNode } from "react";
import { CircularProgress, Backdrop, Alert } from "@mui/material";

import { ProblemDetails } from "@/api/core/api";

type Props<T> = {
  response: {
    isLoading: boolean;
    isFetching: boolean;
    error: ProblemDetails | null;
    data?: T;
  };
  children: (data?: T) => ReactNode;
};

function Data<T>({ response, children }: Props<T>) {
  if (response.isLoading) {
    return (
      <Backdrop open>
        <CircularProgress color="inherit" />
      </Backdrop>
    );
  }

  if (response.error) {
    return (
      <Alert severity="error">
        <strong>{response.error.title}</strong>
        <div>{response.error.detail}</div>
        {response.error.errors && (
          <ul>
            {Object.entries(response.error.errors).map(([field, messages]) => (
              <li key={field}>
                {field}: {messages.join(", ")}
              </li>
            ))}
          </ul>
        )}
      </Alert>
    );
  }

  return (
    <>
      {response.isFetching && <CircularProgress size={20} />}
      {children(response.data)}
    </>
  );
}

export default Data;
