import { getClientAccessToken } from "./accessToken";
import api from "./api";
import urls from "./urls";

export type Category = {
  id: number;
  name: string;
};

export const getCategories = async (): Promise<Category[]> => {
  const resonse = await api(getClientAccessToken()).get(urls.category.list);
  return resonse.data;
};
