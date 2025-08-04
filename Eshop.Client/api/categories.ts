import api from "./core/api";
import urls from "./core/urls";

export type Category = {
  id: number;
  name: string;
};

export const getCategories = async (): Promise<Category[]> => {
  const response = await api().get(urls.category.list);
  return response.data;
};
