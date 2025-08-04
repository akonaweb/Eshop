import { useQuery, UseQueryOptions } from "@tanstack/react-query";
import { ProblemDetails } from "./api";

function useApiQuery<TData>(
  key: string[],
  fetcher: () => Promise<TData>,
  options?: Omit<UseQueryOptions<TData, ProblemDetails>, "queryKey" | "queryFn">
) {
  return useQuery<TData, ProblemDetails>({
    queryKey: key,
    queryFn: fetcher,
    refetchOnWindowFocus: false,
    ...options,
  });
}

export default useApiQuery;
