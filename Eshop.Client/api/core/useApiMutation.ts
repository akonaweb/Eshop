import { useMutation, UseMutationOptions } from "@tanstack/react-query";
import { ProblemDetails } from "./api";

function useApiMutation<TVariables, TData>(
  mutationFn: (variables: TVariables) => Promise<TData>,
  options?: UseMutationOptions<TData, ProblemDetails, TVariables>
) {
  return useMutation<TData, ProblemDetails, TVariables>({
    mutationFn,
    ...options,
  });
}

export default useApiMutation;
