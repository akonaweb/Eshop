import api from "./core/api";
import apiSsr from "./core/apiSsr";
import urls from "./core/urls";
import useApiQuery from "./core/useApiQuery";

export type Category = {
  id: number;
  name: string;
};

export const getCategoriesSsr = async (): Promise<Category[]> => {
  const response = await apiSsr().get(urls.category.list);
  return response.data;
};

export const getCategory = async (categoryId: string): Promise<Category> => {
  if (isNaN(Number(categoryId)))
    return Promise.resolve({ name: "" } as Category);

  const response = await api.get(urls.category.detail(Number(categoryId)));
  return response.data;
};
export function useCategoryDetailQuery(categoryId: string) {
  return useApiQuery(["cart", categoryId], () => getCategory(categoryId));
}

export const addCategory = async (category: Category): Promise<Category> => {
  const response = await api.post(urls.category.add, { name: category.name });
  return response.data;
};

export const updateCategory = async (category: Category): Promise<Category> => {
  const response = await api.put(urls.category.update(category.id), {
    name: category.name,
  });
  return response.data;
};
